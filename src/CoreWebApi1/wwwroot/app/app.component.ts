import { Component } from '@angular/core';



import { HTTP_PROVIDERS } from '@angular/http';


@Component({
    moduleId: module.id,
    selector: 'app-root',
    templateUrl: 'app.component.html',
    styleUrls: ['app.component.css']
})
export class AppComponent {
    // [ ] - property binding
    // ( ) - event binding

    title = 'Jumping';
    mycolor = 'pink';
    myname = 'Doug';

    changeBarColor() {
        this.mycolor = this.mycolor === "orange" ? "green" : "orange";
    }

}


