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
		path: 'shop',
		loadComponent: () =>
			import('./pages/shop/shop').then((module) => module.Shop),
	},
	{
		path: '**',
		redirectTo: 'login',
	}
];
