import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Log } from '../models/log';

@Injectable({
  providedIn: 'root',
})
export class LogService {
  constructor(private http: HttpClient) {}
  path = 'https://localhost:44343/api/logs/';

  getLoglar(): Observable<Log[]> {
    return this.http.get<Log[]>(this.path + 'all');
  }

  getLoglarBySearch(log: Log): Observable<Log[]> {
    return this.http.post<Log[]>(this.path + "search", log);
  }
}
