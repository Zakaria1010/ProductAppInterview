import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list/product-list.component';
import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login/login.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent }, // Route for Product List
    { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
    { path: 'products', component: ProductListComponent }, // Route for Product List
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }