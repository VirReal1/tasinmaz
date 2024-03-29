import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Policies } from '../http/policies';
import { Tasinmaz } from '../models/tasinmaz';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class TasinmazService {
  constructor(private http: HttpClient, private authService: AuthService) {}
  path = 'https://localhost:44343/api/tasinmazlar/';

  getTasinmazlar(): Observable<Tasinmaz[]> {
    if (this.authService.userRole != Policies.AdminPolicy) {
      return this.http.get<Tasinmaz[]>(this.path + 'all/' + this.authService.kullaniciId);
    }
    return this.http.get<Tasinmaz[]>(this.path + 'all/' + 0);
  }

  getTasinmazlarBySearch(tasinmaz: Tasinmaz): Observable<Tasinmaz[]> {
    if (this.authService.userRole != Policies.AdminPolicy) {
      tasinmaz.kullaniciId = this.authService.kullaniciId;
    }
    else{
      tasinmaz.kullaniciId = 0;
    }
    return this.http.post<Tasinmaz[]>(this.path + 'search', tasinmaz);
  }

  addTasinmaz(tasinmaz: Tasinmaz) {
    return this.http.post(this.path, tasinmaz);
  }

  updateTasinmaz(tasinmaz: Tasinmaz) {
    return this.http.put(this.path, tasinmaz);
  }

  deleteTasinmaz(tasinmazId: number) {
    return this.http.delete(this.path + this.authService.kullaniciId + '/' + tasinmazId);
  }
}
