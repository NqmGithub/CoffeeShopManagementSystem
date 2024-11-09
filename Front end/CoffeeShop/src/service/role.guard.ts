import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor( private auth: AuthService, private router: Router ) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const role: string = this.auth.getRoles();
    if(role === 'admin'){
      return true;
    }else{
      this.router.navigate(['unauthorized']);
      return false;
    }
  }
}
