using Course_web_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json;

namespace Course_web_project.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDBContext db;

        private readonly ILogger<HomeController> _logger;

        // буду хранить несколько текущих объектов, чтобы использовать их как навигационное свойство
        private Textures CurrentTexture;
        private Users CurrentUser;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index(string value)
        {
            if (value == null) value = "all";
            HttpContext.Session.SetString("notification_on", "false");
            ViewData["SelectedValue"] = value;
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
            return View(db.Textures.ToList());

        }


        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Active_User") != null)
            {
                // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
                ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
                return RedirectToAction("PersonalUserPage");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult PersonalUserPage()
        {
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            //ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");
            CurrentUser = JsonSerializer.Deserialize<Users>(currentUserJson);
            ViewData["UserName"] = CurrentUser.username;
            ViewData["UserEmail"] = CurrentUser.email;
            ViewData["UserFavorites"] = CurrentUser.Textures.Count;
            //sharedmodels.Comments.Users = db.Users.FirstOrDefault(u => u.ID == CurrentUser.ID);
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
            return View();
        }


        public IActionResult PersonalTexturePage(string value)
        {
            string currentTextureJson;
            // найдем нужный объект в базе
            CurrentTexture = db.Textures.Where(u => u.texture_name == value).FirstOrDefault();
            if (CurrentTexture == null)
            {
                // если не нашлось, значит в value ничего не передали, поэтому возьмем из JSON
                currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                ViewData["PathToImg"] = CurrentTexture.texture_name;
            }
            else
            {
                ViewData["PathToImg"] = value;
            }

            Ratings localRating = db.Ratings.FirstOrDefault(r => r.TexturesId == CurrentTexture.ID);
            if(localRating != null)
            {
                ViewData["TextureRating"] = localRating.Rating;
                // пока так - если кто-то поставил текстуре рейтинг, то отключаем кнопки, больше его менять нельзя (надо будет исправить)
                ViewData["RatingExists"] = "true";
            }
            else
            {
                ViewData["TextureRating"] = "-";
            }

            // при начале работы с текстурой, запомню её как текущую для использования в других методах
            // приходится сериализовать объект в json для хранения
            // чтобы избежать циклического обращения:
            //HttpContext.Session.SetString("CurrentTexture", JsonSerializer.Serialize(CurrentTexture));
            HttpContext.Session.SetString("CurrentTexture", Newtonsoft.Json.JsonConvert.SerializeObject(CurrentTexture, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }));

            ViewData["TextureID"] = CurrentTexture.ID;
            ViewData["TextureName"] = CurrentTexture.texture_name;
            ViewData["TextureType"] = CurrentTexture.texture_type;
            /*Textures localTextures = db.Textures.FirstOrDefault(t => t.ID == CurrentTexture.ID);*/
            ViewData["TextureSeamlessOrPBR"] = CurrentTexture.pbr_or_seamless;
            //var foundRating = db.Ratings.Where(u => u.Textures.ID == CurrentTexture.ID).FirstOrDefault();
            /*            if (CurrentTexture.Ratings != null)
                        {
                            ViewData["TextureRating"] = CurrentTexture.Ratings.Rating;
                        }
                        else
                        {
                            ViewData["TextureRating"] = "-";
                        }*/

            if (HttpContext.Session.GetString("Active_User") != null)
            {
                ViewData["Active_User"] = HttpContext.Session.GetString("Active_User");
            }
            else
            {
                ViewData["Active_User"] = "null";
            }
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");


            // если пользователь залогинен
            currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(currentTextureJson) && !string.IsNullOrEmpty(currentUserJson))
            {
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                CurrentUser = JsonSerializer.Deserialize<Users>(currentUserJson);

                Users localUsers = db.Users.Include(c => c.Textures).FirstOrDefault(u => u.ID == CurrentUser.ID);

                // ищем в базе избранного эту текстуру у текущего пользователя
                if (localUsers.Textures.Count != 0)
                {
                    foreach (var item in localUsers.Textures)
                    {
                        if (item.ID == CurrentTexture.ID)
                        {
                            ViewData["TextureIsFavored"] = "true";
                            break;
                        }
                        else
                        {
                            ViewData["TextureIsFavored"] = "false";
                        }
                    }
                }
                else
                {
                    ViewData["TextureIsFavored"] = "false";
                }
            }
            else
            {
                ViewData["RatingExists"] = "true";
                ViewData["TextureIsFavored"] = "disabled";
            }

            //var textureIsFavored = db.Users.Where(u => u.Textures.ID == CurrentTexture.ID && ).FirstOrDefault();
            /*if (CurrentTexture == null)
            {

            }*/


            /*int CurrentUserID = db.Users.Where(u => u.username == HttpContext.Session.GetString("Active_User")).FirstOrDefault().ID;
             ViewData["CurrentUserID"] = CurrentUserID;*/

            var SharedModelsForView = new SharedModelsForView
            {
                //Comments = new Comments(), // инициализируем объект Comments
                //CommentsList = db.Comments.ToList() // получаем список комментариев из базы данных
                CommentsList = db.Comments.Include(c => c.Users).ToList()
            };

            return View(SharedModelsForView);
        }


        [HttpPost] 
        public async Task<IActionResult> Login(Users user) 
        {
            // поиск введенных данных пользователя в БД
            CurrentUser = db.Users.Include(u => u.Textures).Where(u => u.username == user.username).FirstOrDefault();
            if (CurrentUser != null)
            {
                // Создаем экземпляр PasswordHasher
                var passwordHasher = new PasswordHasher<string>();

                // Используем метод VerifyHashedPassword для сравнения новой строки с сохраненным хешем
                if (passwordHasher.VerifyHashedPassword(null, CurrentUser.password, user.password) == PasswordVerificationResult.Success)
                {
                    // такой пользователь найден в БД и пароль верный

                    HttpContext.Session.SetString("Active_User", user.username);
                    // при начале работы с пользователем, запомню его как текущего для использования в других методах
                    // приходится сериализовать объект в json для хранения
                    // HttpContext.Session.SetString("CurrentUser", JsonSerializer.Serialize(CurrentUser));
                    HttpContext.Session.SetString("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(CurrentUser, new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    }));

                    return RedirectToAction("Index");
                }
                else
                {
                    // Пользователь найден, но ПАРОЛЬ НЕВЕРНЫЙ
                    return RedirectToAction("Registration"); // ДОПИСАТЬ ЧТОБЫ ОСТАВАЛСЯ НА ТОЙ ЖЕ СТРАНИЦЕ, НО ГОВОРИЛ ЧТО ПАРОЛЬ НЕ ВЕРНЫЙ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
            }
            else
            {
                // пользователь не найден - регистрируемся
                return RedirectToAction("Registration");
            } 
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Users user)
        {
            // Создаем экземпляр PasswordHasher
            var passwordHasher = new PasswordHasher<string>();
            // Генерируем хеш пароля
            string hashedPassword = passwordHasher.HashPassword(null, user.password);
            // Передаем в БД хеш, а сам пароль благополучше забываем =)
            user.password = hashedPassword;

            // добавление нового пользователя в БД
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult AddNewTexture()
        {
            if (HttpContext.Session.GetString("notification_on") == "true")
            {
                ViewData["SelectedValue"] = "notification_on";
                HttpContext.Session.SetString("notification_on", "false");
            }
            else
            {
                ViewData["SelectedValue"] = "notification_off";
            }
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTexture(Textures texture)
        {
            HttpContext.Session.SetString("notification_on", "true");
            db.Textures.Add(texture);
            await db.SaveChangesAsync();
            return RedirectToAction("AddNewTexture");
        }

/*        [HttpPost]
        public async Task<IActionResult> NewComment(Comments comment)
        {
            var currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(currentTextureJson) && !string.IsNullOrEmpty(currentUserJson))
            {
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                CurrentUser = JsonSerializer.Deserialize<Users>(currentUserJson);
                comment.Textures = CurrentTexture;
                comment.Users = CurrentUser;

                comment.TexturesId = CurrentTexture.ID; // ПОЧЕМУ МНЕ НУЖНО ДЕЛАТЬ ЭТО ВРУЧНУЮ?
                comment.UsersId = CurrentUser.ID; // ПОЧЕМУ МНЕ НУЖНО ДЕЛАТЬ ЭТО ВРУЧНУЮ?
                *//*comment.TexturesId = comment.Textures.ID; // ПОЧЕМУ МНЕ НУЖНО ДЕЛАТЬ ЭТО ВРУЧНУЮ?
                comment.UsersId = comment.Users.ID; // ПОЧЕМУ МНЕ НУЖНО ДЕЛАТЬ ЭТО ВРУЧНУЮ?*//*
            }


            // добавление нового комментария в БД
            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("PersonalTexturePage");
        }*/



        [HttpPost]
        public async Task<IActionResult> NewComment(SharedModelsForView sharedmodels)
        {
            var currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(currentTextureJson) && !string.IsNullOrEmpty(currentUserJson))
            {
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                CurrentUser = JsonSerializer.Deserialize<Users>(currentUserJson);
                sharedmodels.Comments.Textures = db.Textures.FirstOrDefault(t => t.ID == CurrentTexture.ID);
                sharedmodels.Comments.Users = db.Users.FirstOrDefault(u => u.ID == CurrentUser.ID);
            }


            // добавление нового комментария в БД
            db.Comments.Add(sharedmodels.Comments);
            await db.SaveChangesAsync();
            return RedirectToAction("PersonalTexturePage");
        }

        [HttpPost]
        public async Task<IActionResult> NewRating(SharedModelsForView sharedmodels)
        {
            var currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(currentTextureJson) && !string.IsNullOrEmpty(currentUserJson))
            {
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                sharedmodels.Ratings.Textures = db.Textures.FirstOrDefault(t => t.ID == CurrentTexture.ID);
            }


            // добавление нового рейтинга в БД
            db.Ratings.Add(sharedmodels.Ratings);
            await db.SaveChangesAsync();
            return RedirectToAction("PersonalTexturePage");
        }

        [HttpPost]
        public async Task<IActionResult> AddTextureToFavorite()
        {
            var currentTextureJson = HttpContext.Session.GetString("CurrentTexture");
            var currentUserJson = HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(currentTextureJson) && !string.IsNullOrEmpty(currentUserJson))
            {
                CurrentTexture = JsonSerializer.Deserialize<Textures>(currentTextureJson);
                CurrentUser = JsonSerializer.Deserialize<Users>(currentUserJson);
                Textures texture = db.Textures.FirstOrDefault(t => t.ID == CurrentTexture.ID);
                Users user = db.Users.FirstOrDefault(u => u.ID == CurrentUser.ID);

                // Добавляем/Удаляем текстуру в список избранных текстур пользователя
                // ищем в базе избранного эту текстуру у текущего пользователя
                bool currentTextureIsFavorited = false;
                if (CurrentUser.Textures.Count != 0)
                {
                    foreach (var item in CurrentUser.Textures)
                    {
                        if (item.ID == CurrentTexture.ID) // проверяем есть ли текущая текстура в базе данного пользователя
                        {
                            // если она уже есть в избранном, удаляем
                            currentTextureIsFavorited = true;
                            /*user.Textures.Remove(texture);
                            await db.SaveChangesAsync();*/
                            break;
                        }
                        else
                        {
                            // если ещё нет, то добавляем
                            currentTextureIsFavorited = false;
                            /*user.Textures.Add(texture);
                            await db.SaveChangesAsync();
                            break;*/
                        }
                    }
                }
                if(currentTextureIsFavorited)
                {
                    // если она уже есть в избранном, удаляем
                    user.Textures.Remove(texture);
                    await db.SaveChangesAsync();
                }
                else
                {
                    // если ещё нет, то добавляем
                    user.Textures.Add(texture);
                    await db.SaveChangesAsync();
                }
                // обновим объект CurrentUser в сессии
                HttpContext.Session.SetString("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(CurrentUser, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }));
                /*else
                {
                    // если ещё нет, то добавляем
                    user.Textures.Add(texture);
                    await db.SaveChangesAsync();
                }*/

                // Сохраняем изменения в базе данных
                //await db.SaveChangesAsync();

            }
            return RedirectToAction("PersonalTexturePage");
        }

        public IActionResult Privacy()
        {
            // помещаю имя пользователя во ViewBag для доступа на странице _Layout.cshtml
            ViewBag.ActiveUser = HttpContext.Session.GetString("Active_User");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
