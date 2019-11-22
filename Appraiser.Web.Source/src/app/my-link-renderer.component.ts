import { Component } from '@angular/core';

import { AgRendererComponent } from '@ag-grid-community/angular';

@Component({
    template: `<ul class="ag-nav-ul">
  <li *ngFor="let p of params.routeData" class="ag-nav-li">
    <a [routerLink]="[p.link,params.value]">{{p.label}}</a>
  </li>
</ul>`
})

export class MyLinkRendererComponent implements AgRendererComponent {
    params: any;

    agInit(params: any): void {
        this.params = params;
    }

    refresh(params: any): boolean {
        return false;
    }
}