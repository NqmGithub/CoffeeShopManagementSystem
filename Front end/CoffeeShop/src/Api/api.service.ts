import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../Interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseurl = "https://localhost:44344/api"
  private headerCustom = {}

  constructor(private http: HttpClient) {
    this.headerCustom = {headers: { "Authorization": "Bearer " + localStorage.getItem("token") }}

  }

  login(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/login', data);
  }

  signup(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/signup', data);
  }

  getUserById(id: any): Observable<any>{
    return this.http.get<any>(this.baseurl + `/User/byId/${id}`);
  }
  
  getProducts( search:string,filterCategory:string, filterStatus: string,page: number, pageSize: number,
    sortColumn: string,sortDirection:string): Observable<{ list: Product[], total: number }> {

    return this.http.get<{ list: Product[], total: number }>("https://localhost:44344/api/Product",this.headerCustom);
  }
}
