import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseurl = "https://localhost:44322/api"
  constructor(private http: HttpClient) { }

  login(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/login', data);
  }

  signup(data: any): Observable<any>{
    return this.http.post<any>(this.baseurl + '/Auth/signup', data);
  }
}
