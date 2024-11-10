import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {

  constructor() { }
  private isSidebarOpen = new BehaviorSubject<boolean>(true);
  isSidebarOpen$ = this.isSidebarOpen.asObservable();

  toggleSidebar() {
    this.isSidebarOpen.next(!this.isSidebarOpen.value);
  }
}
