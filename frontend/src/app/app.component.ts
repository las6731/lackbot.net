import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public nightMode = true;
  public showIntro = true;
  public activeTab = 0;
  
  constructor(private router: Router) {
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(e => this.setActiveTab((e as NavigationEnd).url));
    
    this.nightMode = window.localStorage.getItem('lightMode') == null;
    this.showIntro = window.localStorage.getItem('showIntro') == null;
  }

  public setActiveTab(url: string) {
    switch (url) {
      case '/reacts':
        this.activeTab = 1;
        break;
      case '/messages':
        this.activeTab = 2;
        break;
    }
  }

  public hideIntro() {
    this.showIntro = false;
    window.localStorage.setItem('showIntro', '');
  }

  public setNightMode() {
    if (this.nightMode) {
      window.localStorage.removeItem('lightMode');
    } else {
      window.localStorage.setItem('lightMode', '');
    }
  }

  public navigate(route: string) {
    this.router.navigate([ route ]);
  }

}
