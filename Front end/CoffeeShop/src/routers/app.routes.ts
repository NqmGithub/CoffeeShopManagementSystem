import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../views/login/login.component';
import { HomeComponent } from '../views/home/home.component';
import { UnauthorizedComponent } from '../views/unauthorized/unauthorized.component';
import { AdminComponent } from '../views/admin/admin.component';
import { ProductManagerComponent } from '../views/admin/product-manager/product-manager.component';
import { RegisterComponent } from '../views/register/register.component';

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
              
            }
          ]          
    },
    {path:'register', component:RegisterComponent},
    {path:'login', component:LoginComponent},
    {path:'unauthorized', component:UnauthorizedComponent}
];

