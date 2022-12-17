import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AlertifyService } from './services/alertify.service';
import { TasinmazComponent } from './tasinmaz/tasinmaz.component';
import { LogComponent } from './log/log.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { UserComponent } from './user/user.component';
import { TasinmazEditComponent } from './tasinmaz/tasinmaz-edit/tasinmaz-edit.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [AppComponent, NavComponent, LoginComponent, TasinmazComponent, LogComponent, UserComponent, TasinmazEditComponent, UserEditComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, RouterModule.forRoot(appRoutes), ReactiveFormsModule, FormsModule],
  providers: [AlertifyService],
  bootstrap: [AppComponent],
})
export class AppModule {}
