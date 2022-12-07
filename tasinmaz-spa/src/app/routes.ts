import { Routes } from '@angular/router';
import { LogComponent } from './log/log.component';
import { RoleGuard } from './login/role.guard';
import { LoginComponent } from './login/login.component';
import { LoginGuard } from './login/login.guard';
import { RegisterComponent } from './register/register.component';
import { TasinmazComponent } from './tasinmaz/tasinmaz.component';
import { UserOpComponent } from './userOp/userOp.component';

export const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'useroperations/register', component: RegisterComponent, canActivate: [LoginGuard, RoleGuard] },
  { path: 'tasinmazlar', component: TasinmazComponent, canActivate: [LoginGuard] },
  { path: 'logregistry', component: LogComponent, canActivate: [LoginGuard, RoleGuard] },
  { path: 'useroperations', component: UserOpComponent, canActivate: [LoginGuard, RoleGuard] },
  { path: '**', redirectTo: 'tasinmazlar', pathMatch: 'full' },
];
