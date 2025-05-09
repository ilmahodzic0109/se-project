import { Component } from '@angular/core';
import { SearchComponent } from '../search/search.component';
import { ItemHomepageComponent } from '../item-homepage/item-homepage.component';

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [SearchComponent, ItemHomepageComponent],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.css'
})
export class HeroComponent {

}
