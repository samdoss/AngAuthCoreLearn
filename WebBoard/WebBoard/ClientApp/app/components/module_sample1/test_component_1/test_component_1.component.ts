import { Component, OnInit, enableProdMode } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

enableProdMode();

@Component({
    styles: [``],
    templateUrl: './test_component_1.html'
})

export class test_component_1Component implements OnInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private activatedRoute: ActivatedRoute,
    ) { }

    ngOnInit() {

    };

    menu() {
        this.router.navigateByUrl('/module_sample1/menu');
    }
    test_component_1() {
        this.router.navigateByUrl('/module_sample1/test_component_1');
    }
    test_component_2() {
        this.router.navigateByUrl('/module_sample1/test_component_2');
    }

}