import { Routes } from '@angular/router';
import { LoginComponent } from '../views/login/login.component';
import { HomeComponent } from '../views/home/home.component';
import { UnauthorizedComponent } from '../views/unauthorized/unauthorized.component';

export const routes: Routes = [
    {path:'home', component:HomeComponent},
    {path:'login', component:LoginComponent},
    {path:'unauthorized', component:UnauthorizedComponent}
];
