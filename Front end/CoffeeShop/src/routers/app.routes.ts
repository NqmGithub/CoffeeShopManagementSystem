import { Routes } from '@angular/router';
import { LoginComponent } from '../views/login/login.component';
import { HomeComponent } from '../views/home/home.component';
import { UnauthorizedComponent } from '../views/unauthorized/unauthorized.component';
import { RegisterComponent } from '../views/register/register.component';
import { ProfileComponent } from '../views/profile/profile.component';
import { AuthGuard } from '../service/auth.guard';

export const routes: Routes = [
    {path:'', redirectTo:'home', pathMatch:'full'},
    {path:'register', component:RegisterComponent},
    {path:'home', component:HomeComponent},
    {path:'login', component:LoginComponent},
    {path:'profile', component:ProfileComponent, canActivate: [AuthGuard]},
    {path:'unauthorized', component:UnauthorizedComponent}
];
