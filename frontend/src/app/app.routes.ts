import { model } from '@angular/core';
import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		pathMatch: 'full',
		redirectTo: 'catalogo',
	},{
		path: 'senha',
		loadComponent: () =>
			import('./pages/esqueceu-senha/esqueceu-senha').then((module) =>module.SenhaPage),
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
		path: 'perfil',
		loadComponent: () =>
			import('./pages/perfil/perfil').then((module) => module.Perfil),
	},	
	{
		path: 'avaliacoes-produto',
		loadComponent: () =>
			import('./pages/avaliacoes-produto/avaliacoes-produto').then((module) => module.AvaliacoesProduto),
	},
	{
		path: 'carrinho',
		loadComponent: () =>
			import('./pages/carrinho/carrinho').then((module) => module.Carrinho),
	},
	{
		path: '**',
		redirectTo: 'catalogo',
	},
];
