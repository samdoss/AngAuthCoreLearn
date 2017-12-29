import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

//AuthGuard
import { AuthGuard } from '../security/authentication/guard/auth.guard'
import { MenuModule } from './menu/menu.module';

 //Main Component of module
import { module_sample1Component } from './module_sample1.component'; 

//Sub Component for route
import { test_component_1Module } from './test_component_1/test_component_1.module';
import { test_component_2Module } from "./test_component_2/test_component_2.module";

const detailsRoutes: Routes = [
    {
        path: 'module_sample1',
        component: module_sample1Component,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                component: module_sample1Component
            },
            {
                path: 'menu',
                loadChildren: () => MenuModule
            },
            {
                path: 'test_component_1',
                loadChildren: () => test_component_1Module
            },
            {
                path: 'test_component_2',
                loadChildren: () => test_component_2Module
            },
            
        ]
    },
];
@NgModule({
    imports: [
        RouterModule.forChild(detailsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class module_sample1RoutingModule { }
