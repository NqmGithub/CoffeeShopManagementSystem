import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../views/login/login.component';
import { HomeComponent } from '../views/home/home.component';
import { UnauthorizedComponent } from '../views/unauthorized/unauthorized.component';
import { AdminComponent } from '../views/admin/admin.component';
import { ProductManagerComponent } from '../views/admin/product-manager/product-manager.component';
import { RegisterComponent } from '../views/register/register.component';
import { AuthGuard } from '../service/auth.guard';
import { RoleGuardService } from '../service/role.guard';
import { UserManagerComponent } from '../views/admin/user-manager/user-manager.component';
import { ContactComponent } from '../views/contact/contact.component';
import { ProfileComponent } from '../views/profile/profile.component';
import { ContactManagerComponent } from '../views/admin/contact-manager/contact-manager.component';
import { ContactDetailComponent } from '../views/admin/contact-manager/contact-detail/contact-detail.component';
import { NotificationComponent } from '../views/notification/notification.component';
import { HistoryComponent } from '../views/history/history.component';
import { ResetPasswordComponent } from '../views/login/reset-password/reset-password.component';
import { VerifyEmailComponent } from '../views/register/verify-email/verify-email.component';
import { CategoryManagerComponent } from '../views/admin/category-manager/category-manager.component';
import { OrderManagerComponent } from '../views/admin/order-manager/order-dialog/order-manager.component';
import { ProductListComponent } from '../views/product list/product-list.component';
import { ProductDetailComponent } from '../views/productdetail/product-detail.component';
import { CartComponent } from '../views/cart/cart.component';
import { CheckoutComponent } from '../views/checkout/checkout.component';
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
              outlet: 'mainContent'  ,
              canActivate: [RoleGuardService, AuthGuard]     
            },
            {
                path: 'users',
                title: 'User Manager',
                component: UserManagerComponent,
                outlet: 'mainContent',
                canActivate: [RoleGuardService, AuthGuard]
            },
            {
                path: 'contacts',
                title: 'Contact Manager',
                component: ContactManagerComponent,
                outlet: 'mainContent',
                canActivate: [RoleGuardService, AuthGuard]
            },
            {
                path: 'contact-detail/:id',
                title: 'Contact Detail',
                component: ContactDetailComponent,
                outlet: 'mainContent',
                canActivate: [RoleGuardService, AuthGuard]
            },
            {
                path: 'categories',
                title: 'Category Manager',
                component: CategoryManagerComponent,
                outlet: 'mainContent',
                canActivate: [RoleGuardService, AuthGuard]
            },
            {
                path: 'orders',
                title: 'Order Manager',
                component: OrderManagerComponent,
                outlet: 'mainContent',
                canActivate: [RoleGuardService, AuthGuard]
            }
        ],
        canActivate: [RoleGuardService, AuthGuard]
    },
    {path:'contact', component:ContactComponent, canActivate: [AuthGuard]},
    {path:'notification', component:NotificationComponent, canActivate: [AuthGuard]},
    {path:'history', component:HistoryComponent, canActivate: [AuthGuard]},
    {path:'checkout', component:CheckoutComponent, canActivate: [AuthGuard]},
    {path:'register', component:RegisterComponent},
    {path:'login', component:LoginComponent},
    {path:'resetPassword', component:ResetPasswordComponent},
    {path:'verifyEmail', component: VerifyEmailComponent},
    {path:'profile', component:ProfileComponent, canActivate: [AuthGuard]},
    {path:'unauthorized', component:UnauthorizedComponent},
      {
        path: 'productlists',
        component: ProductListComponent
      },
      { path: 'productdetail/:productId', component: ProductDetailComponent },
      { path: 'cart', component: CartComponent, canActivate: [AuthGuard] }
];
