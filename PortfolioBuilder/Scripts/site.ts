import { Home } from './Home';
import { Portfolios } from './Portfolios';
import { Categories } from './Categories';
import { Photos } from './Photos';
import { About } from './About';

class App {

    private static PathName = window.location.pathname;
    private static PathArray = App.PathName.split('/');
    private static Controller = App.PathArray[1].toLowerCase();
    private static CategoryController = App.PathArray.length >= 3 && App.PathArray[2].toLowerCase() == "category";
    private static Id = App.PathArray[App.PathArray.length - 2].toLowerCase();
    private static Action = App.PathArray[App.PathArray.length - 1].toLowerCase();

    public static Route() {
        switch (App.Controller) {
            case '':
            case 'home':
                App.HomeRoute();
                break;
            case 'portfolios':
                if (!this.CategoryController) App.PortfoliosRoute();
                else App.CategoriesRoute();
                break;
            case 'photos':
                App.PhotosRoute();
                break;
            case 'about':
                App.AboutRoute();
                break;
        }
    }

    private static HomeRoute() {
        switch (App.Action) {
            case '':
                Home.Index();
                break;
            case 'edit':
                Home.Edit();
                break;
        }
    }

    private static PortfoliosRoute() {
        switch (App.Action) {
            case 'portfolios':
                Portfolios.Index();
                break;
            case 'add':
                Portfolios.Add();
                break;
            case 'edit':
                if (App.Id == App.Controller) {
                    Portfolios.IndexEdit();
                }
                else {
                    Portfolios.Edit();
                }
                break;
            case 'delete':
                Portfolios.Delete();
                break;
            default:
                Portfolios.Details();
                break;
        }
    }

    private static CategoriesRoute() {
        switch (App.Action) {
            case 'all':
                Categories.Index();
                break;
            case 'add':
                Categories.Add();
                break;
            case 'edit':
                if (App.Id == "all") {
                    Categories.IndexEdit();
                }
                else {
                    Categories.Edit();
                }
                break;
            case 'delete':
                Categories.Delete();
                break;
            default:
                Categories.Details();
                break;
        }
    }

    private static PhotosRoute() {
        switch (App.Action) {
            case 'photos':
                Photos.Index();
                break;
            case 'add':
                Photos.Add();
                break;
            case 'edit':
                Photos.Edit();
                break;
            case 'delete':
                Photos.Delete();
                break;
        }
    }

    private static AboutRoute() {
        switch (App.Action) {
            case 'about':
                About.Index();
                break;
            case 'edit':
                About.Edit();
                break;
        }
    }
};

App.Route();