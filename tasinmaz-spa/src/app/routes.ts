import { Routes } from '@angular/router';
import { LogComponent } from './log/log.component';
import { TasinmazComponent } from './tasinmaz/tasinmaz.component';
import { UserComponent } from './user/user.component';
import { LoginGuard } from './guards/login.guard';
import { RoleGuard } from './guards/role.guard';
import { LogoutGuard } from './guards/logout.guard';
import { LoginComponent } from './login/login.component';

export const appRoutes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [LogoutGuard] },
  { path: 'users', component: UserComponent, canActivate: [LoginGuard, RoleGuard] },
  { path: 'tasinmazlar', component: TasinmazComponent, canActivate: [LoginGuard] },
  { path: 'logs', component: LogComponent, canActivate: [LoginGuard, RoleGuard] },
  { path: '**', redirectTo: 'tasinmazlar', pathMatch: 'full' },
];
