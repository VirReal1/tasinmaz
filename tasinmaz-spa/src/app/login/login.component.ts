import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginKullanici } from 'src/app/models/loginKullanici';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [AuthService],
})
export class LoginComponent implements OnInit {
  constructor(private authService: AuthService, private formBuilder: FormBuilder) {}
  loginKullanici: any = {};
  loginForm: FormGroup;

  ngOnInit() {
    this.createLoginForm();
  }

  login() {
    if (this.loginForm.valid) {
      let loginParameters: LoginKullanici;
      loginParameters = Object.assign({}, this.loginForm.value);
      this.authService.login(loginParameters);
    }
  }

  createLoginForm() {
    this.loginForm = this.formBuilder.group({
      email: [],
      password: [],
    });
  }

  formResetle() {
    this.loginForm.controls.email.reset();
    this.loginForm.controls.password.reset();
  }
}
