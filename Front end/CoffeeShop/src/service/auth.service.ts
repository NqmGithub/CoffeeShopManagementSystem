import { Injectable } from '@angular/core';
import { ApiService } from '../Api/api.service';
import { Router } from '@angular/router';
import { User } from '../Interfaces/user';
import { BehaviorSubject, Observable } from 'rxjs';

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
            this.apiService.getUserById(userId).subscribe((user) => {
                this.currentUser.next(user);
            });
        }
    }

    getCurrentUser(){
        return this.currentUser;
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

    login(email: string, password: string) {
        let payload = { email: email, password: password };
        this.apiService.login(payload).subscribe(
            (response) => {
                localStorage.setItem('token', response.token);
                this.router.navigate(['/home']);
            },
            (error) => {
                console.error('Error fetching data', error);
            }
        );
    }

    signup(user: User) {
        let payload = {
            username: user.userName,
            password: user.password,
            phoneNumber: user.phoneNumber,
            email: user.email,
            address: user.address,
        };
        this.apiService.signup(payload).subscribe(
            (response) => {
                localStorage.setItem('token', response.token);
                this.router.navigate(['/home']);
            },
            (error) => {
                console.error('Error fetching data', error);
            }
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
