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
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  isAdmin: boolean = false; // Default isAdmin is false
  selectedProduct: Product | null = null;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    // Check role from localStorage and update isAdmin accordingly
    const storedRole = localStorage.getItem('role');
    this.isAdmin = storedRole === 'admin'; // Update to check for 'admin' role
    this.loadProducts(); // Load products initially
  }

  // Toggle role for testing purposes (can be removed in production)
  toggleRole(): void {
    if (this.isAdmin) {
      this.isAdmin = false;
      localStorage.setItem('role', 'user'); // Store 'user' role in localStorage
      console.log('Switched to User role.');
    } else {
      this.isAdmin = true;
      localStorage.setItem('role', 'admin'); // Store 'admin' role in localStorage
      console.log('Switched to Admin role.');
    }
  }

  // Select product for editing or viewing details
  selectProduct(product: Product): void {
    this.selectedProduct = { ...product };
  }

  // Load products from the service
  loadProducts(): void {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
    });
  }

  // Update product only if the user is an admin
  updateProduct(id: number, updatedProduct: Product): void {
    if (this.isAdmin) {
      this.productService.updateProduct(id, updatedProduct).subscribe(() => {
        this.loadProducts(); // Reload products after update
      });
    } else {
      alert('Vous devez être admin pour modifier les produits.'); // Alert if user is not admin
    }
  }

  // Cancel the product selection (clear the selected product)
  cancel(): void {
    this.selectedProduct = null;
  }
}