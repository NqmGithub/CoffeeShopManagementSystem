import { HttpClient, HttpEvent, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Product } from '../Interfaces/product';
import { CreateProduct } from '../Interfaces/createProduct';
import { User } from '../Interfaces/user';
import { I } from '@angular/cdk/keycodes';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseurl = "https://localhost:44344/api"
  private headerCustom = {}

  constructor(private http: HttpClient) {
    this.headerCustom = { headers: { "Authorization": "Bearer " + localStorage.getItem("token") } }
  }

  //auth
  login(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/login', data);
  }

  signup(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/signup', data);
  }

  //user
  getUserById(id: any): Observable<any>{
    return this.http.get<any>(this.baseurl + `/User/byId/${id}`, this.headerCustom);
  }

  getUserCount(): Observable<any>{
    return this.http.get<any>(this.baseurl + '/User/count', this.headerCustom);
  }

  addUser(user: User): Observable<any>{
    return this.http.post<User[]>(this.baseurl + `/User`, user, this.headerCustom);
  }

  updateUser(id: string, user: User): Observable<any>{
    return this.http.put<User[]>(this.baseurl + `/User/${id}`, user, this.headerCustom);
  }

  makeUserInactive(id: string): Observable<any>{
    return this.http.patch<User[]>(this.baseurl + `/User/${id}`, this.headerCustom);
  }

  searchUser(keyword: string, status: string, pageNumber: number, pageSize: number) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('status', status);
  
    if (keyword) {
      params = params.set('keyword', keyword);
    }
  
    return this.http.get<User[]>(`${this.baseurl}/User/search`, { params});
  }

  //Product
  getProducts(search: string, filterCategory: string, filterStatus: string, page: number, pageSize: number,
    sortColumn: string, sortDirection: string): Observable<{ list: Product[], total: number }> {

    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('sortColumn', sortColumn)
      .set('sortDirection', sortDirection);

    if (search) {
      params = params.set('search', search);
    }

    if (filterCategory) {
      params = params.set('filterCategory', filterCategory);
    }

    if (filterStatus) {
      params = params.set('filterStatus', filterStatus);
    }

    return this.http.get<{ list: Product[], total: number }>(`${this.baseurl}/Product`, {
      ...this.headerCustom,
      params: params
    });
  }

  getAllCateogryNames(): Observable<string[]> {
    return this.http.get<string[]>(this.baseurl + '/Category/name',this.headerCustom);
  }

  uploadImage(name:string, formData: FormData, folder: string): Observable<HttpEvent<any>>{
    let params = new HttpParams()
    .set('name', name)
    .set('folder', folder);
    
    return this.http.post<any>(`${this.baseurl}/Image/upload`,formData,{
      params:params
    });
  }

  checkProductNameExist(productName:string):Observable<boolean>{
    return this.http.get<boolean>(`${this.baseurl}/Product/checkName?productName=${productName}`,this.headerCustom);
  }

  postProduct(createProduct: CreateProduct): Observable<boolean>{
    return this.http.post<boolean>(`${this.baseurl}/Product`,createProduct,this.headerCustom);
  }

  putProduct(id:string, updateProduct:Product):Observable<boolean>{
    return this.http.put<boolean>(`${this.baseurl}/Product/update/`+id,updateProduct,this.headerCustom);
  }

  changeStatus(id:string, status:string):Observable<boolean>{
    return this.http.put<boolean>(`${this.baseurl}/Product/`+id+'?status='+status,this.headerCustom);
  }

}
