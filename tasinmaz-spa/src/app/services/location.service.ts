import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Il } from '../models/il';
import { Ilce } from '../models/ilce';
import { Mahalle } from '../models/mahalle';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  constructor(private http: HttpClient) {}
  path = 'https://localhost:44343/api/locations/';

  getIller(): Observable<Il[]> {
    return this.http.get<Il[]>(this.path + 'il');
  }

  getIlceler(ilAdi): Observable<Ilce[]> {
    return this.http.get<Ilce[]>(this.path + 'ilce/' + ilAdi);
  }

  getMahalleler(ilceAdi): Observable<Mahalle[]> {
    return this.http.get<Mahalle[]>(this.path + 'mahalle/' + ilceAdi);
  }
}
