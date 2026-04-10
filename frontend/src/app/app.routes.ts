import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		pathMatch: 'full',
		redirectTo: 'login',
	},
	{
		path: 'login',
		loadComponent: () =>
			import('./pages/login/login').then((module) => module.LoginPage),
	},
	{
		path: 'register',
		loadComponent: () =>
			import('./pages/register/register').then((module) => module.RegisterPage),
	},
	{
		path: '**',
		redirectTo: 'login',
	},
];
