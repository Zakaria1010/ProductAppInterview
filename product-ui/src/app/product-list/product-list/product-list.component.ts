import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle'; 

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule, 
    MatButtonModule, 
    MatCardModule, 
    MatFormFieldModule, 
    MatInputModule,
    MatSlideToggleModule,
    FormsModule,
  ],
  providers: [ProductService],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  isAdmin: boolean = true;
  selectedProduct: Product | null = null;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    const storedRole = localStorage.getItem('role')
    this.isAdmin = storedRole === 'Admin'
    this.loadProducts()
  }

  toggleRole(): void {
    if (this.isAdmin) {
      this.isAdmin = false;
      localStorage.setItem('role', 'User');
      console.log('Switched to User role.');
    } else {
      this.isAdmin = true;
      localStorage.setItem('role', 'Admin');
      console.log('Switched to Admin role.');
    }
  }

  selectProduct(product: Product): void {
    this.selectedProduct = { ...product };
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe((data) => {
      this.products = data
    })
  }

  updateProduct(id: number, updatedProduct: Product): void {
    if (this.isAdmin) {
      this.productService.updateProduct(id, updatedProduct).subscribe(() => {
        this.loadProducts();
      })
    } else {
      alert('Vous devez être admin pour modifier les produits.')
    }
  }

  cancel(): void {
    this.selectedProduct = null
  }
}
