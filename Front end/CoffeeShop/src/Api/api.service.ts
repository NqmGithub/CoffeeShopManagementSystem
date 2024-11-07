import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../Interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseurl = "https://localhost:44322/api"
  private headerCustom = {}

  constructor(private http: HttpClient) {
    this.headerCustom = {headers: { "Authorization": "Bearer " + localStorage.getItem("token") }}

  }

  login(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/User/login', data);
  }

  getProducts(): Observable<Product[]> {

    return this.http.get<Product[]>("https://localhost:44344/api/Product",this.headerCustom);
  }
}
