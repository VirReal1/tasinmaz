import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddKullanici } from '../models/addKullanici';
import { ShowKullanici } from '../models/showKullanici';
import { UpdateKullanici } from '../models/updateKullanici';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}
  path = 'https://localhost:44343/api/users/';

  getKullanicilar(): Observable<ShowKullanici[]> {
    return this.http.get<ShowKullanici[]>(this.path + 'all/' + this.authService.kullaniciId);
  }

  getKullanicilarBySearch(kullanici): Observable<ShowKullanici[]> {
    return this.http.post<ShowKullanici[]>(this.path + 'search', kullanici);
  }

  addKullanici(kullanici: AddKullanici) {
    return this.http.post(this.path, kullanici);
  }

  updateKullanici(kullanici: UpdateKullanici) {
    return this.http.put(this.path, kullanici);
  }

  deleteKullanici(kullaniciId: number) {
    return this.http.delete(this.path + this.authService.kullaniciId + "/" + kullaniciId);
  }
}
