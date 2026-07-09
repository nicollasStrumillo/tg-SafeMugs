import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		pathMatch: 'full',
		redirectTo: 'catalogo',
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
		path: 'catalogo',
		loadComponent: () =>
			import('./pages/catalogo/catalogo').then((module) => module.Catalogo),
	},
	{
		path: 'score-board',
		loadComponent: () =>
			import('./pages/score-board/score-board').then((module) => module.ScoreBoard),
	},
	{
		path: '**',
		redirectTo: 'catalogo',
	}
];
