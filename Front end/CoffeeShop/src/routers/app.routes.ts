import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../views/login/login.component';
import { HomeComponent } from '../views/home/home.component';
import { UnauthorizedComponent } from '../views/unauthorized/unauthorized.component';
import { AdminComponent } from '../views/admin/admin.component';
import { ProductManagerComponent } from '../views/admin/product-manager/product-manager.component';
import { RegisterComponent } from '../views/register/register.component';
// import { ProfileComponent } from '../views/profile/profile.component';
import { AuthGuard } from '../service/auth.guard';
import { RoleGuardService } from '../service/role.guard';
import { UserManagerComponent } from '../views/admin/user-manager/user-manager.component';
import { ContactComponent } from '../views/contact/contact.component';
import { ProfileComponent } from '../views/profile/profile.component';
import { ResetPasswordComponent } from '../views/login/reset-password/reset-password.component';
import { VerifyEmailComponent } from '../views/register/verify-email/verify-email.component';

export const routes: Routes = [
    {
        path:'home',
        title:'CoffeeShop',
        component:HomeComponent
    },
    {
        path:'admin',
        title:'Admin',
        component:AdminComponent,
        children: [
            {
              path: 'products',
              title: 'Product Manager',
              component: ProductManagerComponent,
              outlet: 'mainContent'       
            },
            {
                path: 'users',
                title: 'User Manager',
                component: UserManagerComponent,
                outlet: 'mainContent'
            }
        ],
        // canActivate: [RoleGuardService]
    },
    {path:'contact', component:ContactComponent},
    {path:'register', component:RegisterComponent},
    {path:'login', component:LoginComponent},
    {path:'resetPassword', component:ResetPasswordComponent},
    {path:'verifyEmail', component: VerifyEmailComponent},
    {path:'profile', component:ProfileComponent, canActivate: [AuthGuard]},
    {path:'unauthorized', component:UnauthorizedComponent}
];

