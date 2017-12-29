import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'module_sample1',
    templateUrl: './module_sample1.component.html',
    styleUrls: ['./module_sample1.component.css']

})

export class module_sample1Component implements OnInit {
    constructor(
        private router: Router,
    ) {
        console.log('module_sample1 Component');
    }

    ngOnInit(): void {
        this.router.navigate(['/module_sample1/menu']);
    }

}