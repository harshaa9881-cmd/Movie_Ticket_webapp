import { Routes } from '@angular/router';

import { Home } from './features/home/home';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';

import { TheatreList } from './features/customer/theatre-list/theatre-list';
import { ShowList } from './features/customer/show-list/show-list';

import { MovieList } from './features/customer/movie-list/movie-list';
import { MovieDetails } from './features/customer/movie-details/movie-details';

import { Dashboard as AdminDashboard } from './features/admin/dashboard/dashboard';
import { Dashboard as CustomerDashboard } from './features/customer/dashboard/dashboard';

import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { customerGuard } from './core/guards/customer.guard';

export const routes: Routes = [

  {
    path: '',
    component: Home
  },

  {
    path: 'login',
    component: Login
  },

  {
    path: 'register',
    component: Register
  },

  {
    path: 'admin',
    component: AdminDashboard,
    canActivate: [authGuard, adminGuard]
  },

  {
    path: 'customer',
    component: CustomerDashboard,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/movies',
    component: MovieList,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/movies/:id',
    component: MovieDetails,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/theatres',
    component: TheatreList,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/theatres/:theatreId/shows',
    component: ShowList,
    canActivate: [authGuard, customerGuard]
  },

  {
    path: '**',
    redirectTo: ''
  }

];
