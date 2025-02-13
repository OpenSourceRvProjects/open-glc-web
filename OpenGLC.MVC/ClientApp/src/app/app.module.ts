import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AppRoutingModule } from './app-routing.module';
import { NewRegisterComponent } from './Pages/Register/new-register.component';
import { LoginComponent } from './Pages/Login/login.component';
import { MealEventComponent } from './Pages/MealEvent/meal-event.component';
import { EventsComponent } from './Pages/EventsList/events.component';
import { GoogleChartsModule } from 'angular-google-charts';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/compiler';
import { LineChartComponent } from './Components/Graphs/line-chart.component';
import { BarChartComponent } from './Components/Graphs/bar-chart.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    NewRegisterComponent,
    LoginComponent,
    MealEventComponent,
    EventsComponent,
    LineChartComponent,
    BarChartComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    GoogleChartsModule
    //RouterModule.forRoot([
    //  { path: '', component: HomeComponent, pathMatch: 'full' },
    //  { path: 'counter', component: CounterComponent },
    //  { path: 'fetch-data', component: FetchDataComponent },
    //])
  ],
  //schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
