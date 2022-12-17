import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tasinmaz } from '../models/tasinmaz';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class TasinmazService {
  constructor(private http: HttpClient, private authService: AuthService) {}
  path = 'https://localhost:44343/api/tasinmazlar/';

  getTasinmazlar(): Observable<Tasinmaz[]> {
    if (this.authService.adminMi === false) {
      return this.http.get<Tasinmaz[]>(this.path + 'all/' + this.authService.kullaniciId);
    }
    return this.http.get<Tasinmaz[]>(this.path + 'all/' + 0);
  }

  getTasinmazlarBySearch(tasinmaz): Observable<Tasinmaz[]> {
    if (this.authService.adminMi === false) {
      tasinmaz.logKullaniciId = this.authService.kullaniciId;
    }
    return this.http.post<Tasinmaz[]>(this.path + 'search', tasinmaz);
  }

  addTasinmaz(tasinmaz) {
    return this.http.post(this.path, tasinmaz);
  }

  updateTasinmaz(tasinmaz) {
    return this.http.put(this.path, tasinmaz);
  }

  deleteTasinmaz(tasinmaz) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: tasinmaz,
    };
    return this.http.delete(this.path, options);
  }
}
