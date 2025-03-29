import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list/product-list.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
    { path: 'products', component: ProductListComponent }, // Route for Product List
    { path: '', redirectTo: '/products', pathMatch: 'full' }, // Default route
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }