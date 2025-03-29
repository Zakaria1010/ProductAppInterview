import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = `${environment.apiUrl}/api/products`;

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  updateProduct(id: number, product: Product): Observable<void> {
    // Get the role from localStorage (default to 'user' if not found)
    let role = localStorage.getItem('role') || 'user'
  
    // Retrieve the JWT token from localStorage
    let token = localStorage.getItem('token')
  
    // Check if the token exists, if not handle the case (optional)
    if (!token) {
      // You can handle this by returning an error, throwing an exception, or redirecting to login
      throw new Error('No authentication token found.')
    }
  
    // Prepare the headers with the Authorization and Role fields
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Role': role  // Add the role to the headers
    });
  
    // Send the HTTP PUT request with the product data and the headers
    return this.http.put<void>(`${this.apiUrl}/${id}`, product, { headers })
  }
}
