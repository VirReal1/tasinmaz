import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AlertifyService } from './services/alertify.service';
import { TasinmazComponent } from './tasinmaz/tasinmaz.component';
import { LogComponent } from './log/log.component';
import { UserOpComponent } from './userOp/userOp.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';

@NgModule({
  declarations: [AppComponent, NavComponent, LoginComponent, RegisterComponent, TasinmazComponent, LogComponent, UserOpComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, RouterModule.forRoot(appRoutes), ReactiveFormsModule, FormsModule],
  providers: [AlertifyService],
  bootstrap: [AppComponent],
})
export class AppModule {}
