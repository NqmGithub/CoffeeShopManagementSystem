import { HttpClient, HttpEvent, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Product } from '../Interfaces/product';
import { CreateProduct } from '../Interfaces/createProduct';
import { User } from '../Interfaces/user';
import { Contact } from '../Interfaces/contact';
import { Order } from '../Interfaces/order';
import { CreateContact } from '../Interfaces/createContact';
import { UpdateContactResponse } from '../Interfaces/updateContactResponse';
import { ProblemType } from '../Interfaces/category';


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

  uploadImage(name:string,file: File,folder:string): Observable<HttpEvent<any>>{
    const formData: FormData = new FormData();
    formData.append('file', file);
    let params = new HttpParams()
    .set('name', name)
    .set('folder', folder);
    
    return this.http.post<any>(`${this.baseurl}/Image/upload`,formData,{
      params:params,
      ...this.headerCustom
    });
  }

  updateNameImage(oldName:string,newName: string){
    let params = new HttpParams()
    .set('oldName',oldName)
    .set('newName',newName)

    return this.http.put<any>(`${this.baseurl}/Image/update-name`,{},{
      params:params,
      ...this.headerCustom
    })
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
// Product
getProductLists(
  search: string,
  category: string,
  minPrice: number | null,
  maxPrice: number | null,
  page: number,
  pageSize: number,
  sortBy: string,
  isDescending: boolean
): Observable<any> {
  // Thiết lập tham số, tránh gửi các tham số trống
  const params: any = {
    search: search || '',  // Nếu không có giá trị, sẽ gửi chuỗi rỗng
    category: category || '',  // Nếu không có giá trị, sẽ gửi chuỗi rỗng
    page: page.toString(),
    pageSize: pageSize.toString(),
    sortBy: sortBy || 'Id',  // Nếu không có giá trị sắp xếp, mặc định là 'Id'
    isDescending: isDescending.toString()  // Chuyển true/false thành chuỗi 'true'/'false'
  };

  // Chỉ thêm tham số minPrice và maxPrice nếu có giá trị hợp lệ
  if (minPrice !== null) {
    params.minPrice = minPrice.toString();
  }
  if (maxPrice !== null) {
    params.maxPrice = maxPrice.toString();
  }

  return this.http.get<{ list: Product[], total: number }>(`${this.baseurl}/UserProduct`, {
    ...this.headerCustom,
    params: params
  });
}

// Lấy danh sách tất cả tên danh mục
getAllCategoryNames(): Observable<string[]> {
  return this.http.get<string[]>(
    `${this.baseurl}/Category/name`,
    this.headerCustom
  );
}
  //feedback
  getAllContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.baseurl + '/Contact',this.headerCustom);
  }

  getContactById(id: string): Observable<Contact> {
    return this.http.get<Contact>(this.baseurl + '/Contact/'+id,this.headerCustom);
  }

  postContact(createContact: CreateContact): Observable<boolean>{
    return this.http.post<boolean>(`${this.baseurl}/Contact`,createContact,this.headerCustom);
  }

  putContact(id: string,updateContactResponse: UpdateContactResponse): Observable<boolean>{
    return this.http.post<boolean>(`${this.baseurl}/Contact/`+id,updateContactResponse,this.headerCustom);
  }

  //problemType
  //feedback
  getAllProblemTypes(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.baseurl + '/ProblemType',this.headerCustom);
  }

  getProblemTypeById(id: string): Observable<Contact> {
    return this.http.get<Contact>(this.baseurl + '/ProblemType/'+id,this.headerCustom);
  }

  postProblemType(problemType: ProblemType): Observable<boolean>{
    return this.http.post<boolean>(`${this.baseurl}/ProblemType`,problemType,this.headerCustom);
  }

  putProblemType(id: string,problemType: ProblemType): Observable<boolean>{
    return this.http.post<boolean>(`${this.baseurl}/ProblemType/`+id,problemType,this.headerCustom);
  }
 // Order Management
 getOrders(search: string, status: string, sortColumn: string, sortDirection: string, pageNumber: number, pageSize: number): Observable<any> {
  let params = new HttpParams()
    .set('search', search)
    .set('status', status)
    .set('sortColumn', sortColumn)
    .set('isDescending', sortDirection === 'desc' ? 'true' : 'false')
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString());

  return this.http.get<any>(`${this.baseurl}/Order`, { params, ...this.headerCustom });
}


updateOrderStatus(orderId: string): Observable<any> {
  if (!orderId) {
    console.error('Invalid orderId');
    return throwError('Invalid orderId');
  }

  const url = `${this.baseurl}/api/Order/${orderId}/toggle-status`;

  return this.http.put<any>(url, null, this.headerCustom);
}


getOrderById(orderId: string): Observable<Order> {
  return this.http.get<Order>(`${this.baseurl}/Order/${orderId}`, this.headerCustom);
}

createOrder(order: Order): Observable<Order> {
  return this.http.post<Order>(`${this.baseurl}/Order`, order, this.headerCustom);
}

updateOrder(orderId: string, order: Order): Observable<Order> {
  return this.http.put<Order>(`${this.baseurl}/Order/${orderId}`, order, this.headerCustom);
}

deleteOrder(orderId: string): Observable<boolean> {
  return this.http.delete<boolean>(`${this.baseurl}/Order/${orderId}`, this.headerCustom);
}
// Đặt giá trị vào cookie
setCookie(name: string, value: string, days: number): void {
  const date = new Date();
  date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000)); // Thêm số ngày vào hiện tại
  const expires = `expires=${date.toUTCString()}`;
  document.cookie = `${name}=${value};${expires};path=/`;
}

// Lấy giá trị của cookie
getCookie(name: string): string | null {
  const nameEq = `${name}=`;
  const cookies = document.cookie.split(';');
  for (let i = 0; i < cookies.length; i++) {
    let c = cookies[i].trim();
    if (c.indexOf(nameEq) === 0) {
      return c.substring(nameEq.length, c.length);
    }
  }
  return null; // Nếu không tìm thấy cookie
}

// Xóa cookie
deleteCookie(name: string): void {
  document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/`;
}
getProductById(productId: string): Observable<Product> {
  return this.http.get<Product>(`${this.baseurl}/UserProductDetail/${productId}`);
}

// Lấy sản phẩm liên quan theo productId
getRelatedProducts(productId: string): Observable<Product[]> {
  return this.http.get<Product[]>(`${this.baseurl}/UserProductDetail/related/${productId}`);
}
}
