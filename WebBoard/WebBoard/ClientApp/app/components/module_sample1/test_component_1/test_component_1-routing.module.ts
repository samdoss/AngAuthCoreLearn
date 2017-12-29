import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { test_component_1Component } from './test_component_1.component';

const detailsRoutes: Routes = [
    {
        path: '', component: test_component_1Component,
        children: [
            {
                path: '',
                redirectTo: '/module_sample1/test_component_1',
                pathMatch: 'full'
            },
            {
                path: 'test_component_1',
                component: test_component_1Component
            },
        ]
    },
    { path: "**", redirectTo: '/login' }
];
@NgModule({
    imports: [
        RouterModule.forChild(detailsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class test_component_1RoutingModule { }
