import { Routes } from '@angular/router';

import { Home } from './features/home/home';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';

import { TheatreList } from './features/customer/theatre-list/theatre-list';
import { ShowList } from './features/customer/show-list/show-list';

import { BookingConfirmation } from './features/customer/booking-confirmation/booking-confirmation';
import { BookingHistory } from './features/customer/booking-history/booking-history';

import { MovieList } from './features/customer/movie-list/movie-list';
import { MovieDetails } from './features/customer/movie-details/movie-details';

import { SeatSelection } from './features/customer/seat-selection/seat-selection';
import { MovieManagement } from './features/admin/movie-management/movie-management';

import { Dashboard as AdminDashboard } from './features/admin/dashboard/dashboard';
import { Dashboard as CustomerDashboard } from './features/customer/dashboard/dashboard';

import { TheatreManagement } from './features/admin/theatre-management/theatre-management';
import { ScreenManagement } from './features/admin/screen-management/screen-management';
import { SeatManagement } from './features/admin/seat-management/seat-management';

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
    path: 'customer/shows/:showId/seats',
    component: SeatSelection,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/booking-confirmation',
    component: BookingConfirmation,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'customer/bookings',
    component: BookingHistory,
    canActivate: [authGuard, customerGuard]
  },
  {
    path: 'admin/movies',
    component: MovieManagement,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'admin/theatres',
    component: TheatreManagement,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'admin/screens',
    component: ScreenManagement,
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'admin/seats',
    component: SeatManagement,
    canActivate: [authGuard, adminGuard]
  },

  {
    path: '**',
    redirectTo: ''
  }

];
