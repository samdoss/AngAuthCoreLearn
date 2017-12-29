import { Component, OnInit, enableProdMode } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

enableProdMode();

@Component({
    styles: [``],
    templateUrl: './sub_test_component_2_1.html'
})

export class sub_test_component_2_1Component implements OnInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private activatedRoute: ActivatedRoute,
    ) { }

    ngOnInit() {

    };

}