import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddUpdateKullanici } from '../models/addUpdateKullanici';
import { ShowDeleteKullanici } from '../models/showDeleteKullanici';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}
  path = 'https://localhost:44343/api/users/';

  getKullanicilar(): Observable<ShowDeleteKullanici[]> {
    return this.http.get<ShowDeleteKullanici[]>(this.path + 'all/' + this.authService.kullaniciId);
  }

  getKullanicilarBySearch(kullanici): Observable<ShowDeleteKullanici[]> {
    return this.http.post<ShowDeleteKullanici[]>(this.path + 'search', kullanici);
  }

  addKullanici(kullanici: AddUpdateKullanici) {
    return this.http.post(this.path + 'register', kullanici);
  }

  updateKullanici(kullanici: AddUpdateKullanici) {
    return this.http.put(this.path, kullanici);
  }

  deleteKullanici(kullanici: ShowDeleteKullanici) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: kullanici,
    };
    return this.http.delete(this.path, options);
  }
}
