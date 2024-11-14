import { Injectable } from '@angular/core';
import { ApiService } from '../Api/api.service';
import { Router } from '@angular/router';
import { User } from '../Interfaces/user';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuthService {

    private currentUser = new BehaviorSubject<User | null>(null);
    currentUser$ = this.currentUser.asObservable();

    constructor(private apiService: ApiService, private router: Router) {
        this.loadUser();
    }

    loadUser(){
        const token = localStorage.getItem('token');
        if (token) {
            const userId = this.getId();
            this.apiService.getUserById(userId).pipe(catchError(e => {
                console.error(e.message);
                this.currentUser.next(null);
                return throwError(() => new Error('An error occurred while fetching data.'));
            })).subscribe((user) => {
                this.currentUser.next(user);
                error: () => this.router.navigate(['/login'])
            });
        }
    }

    getCurrentUser(){
        if (!this.currentUser.getValue()) {
            this.loadUser();
        }
        return this.currentUser$;
    }

    isLoggedIn(): boolean {
        if (localStorage.getItem('token')) {
            let tokenData = this.parseJwt(localStorage.getItem('token') ?? '');
            const currentTime = Math.floor(Date.now() / 1000);
            if (tokenData.exp > currentTime) {
                return true;
            } else {
                this.logout();
                return false;
            }
        }
        return false;       
    }

    login(email: string, password: string): Observable<void>{
        let payload = { email: email, password: password };
        return this.apiService.login(payload).pipe(
            tap(response => {
                localStorage.setItem("token", response.token);
                localStorage.setItem("email",email);
                this.router.navigate(['/home']);
              }),
              catchError(error => {
                console.error("Error:", error);
                return throwError(() => error.error || `Error`);
              })
        );
    }

    signup(user: User): Observable<void>{
        let payload = {
            username: user.userName,
            password: user.password,
            phoneNumber: user.phoneNumber,
            email: user.email,
            address: user.address,
        };
        return this.apiService.signup(payload).pipe(
            tap(response => {
                localStorage.setItem('token', response.token);
                this.router.navigate(['/home']);
            }),
            catchError(error => {
                console.error("Error:", error);
                return throwError(() => error.error || `Error`);
            })
        );
    }

    logout() {
        localStorage.clear();
        this.currentUser.next(null);
        this.router.navigate(['login']);
    }

    getRoles() {
        const token = localStorage.getItem('token');
        const decodedToken = this.parseJwt(token ?? '');
        const userRole: string = decodedToken?.role;
        return userRole;
    }

    getId() {
        const token = localStorage.getItem('token');
        const decodedToken = this.parseJwt(token ?? '');
        const userId: string = decodedToken?.sub;
        return userId;
    }

    parseJwt(token: string) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(function (c) {
                    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
                })
                .join('')
        );

        return JSON.parse(jsonPayload);
    }
}
