import { Component, OnInit } from '@angular/core';
import { Policies } from '../http/policies';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [AuthService],
})
export class NavComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit() {}

  logout() {
    this.authService.logout();
  }

  adminMi() {
    if (this.authService.userRole == Policies.AdminPolicy) {
      return true;
    }
    return false;
  }
}
