using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BBMS.Controllers
{
    [Authorize]
    public class BDonorsController : Controller
    {
        private BBManagementEntities db = new BBManagementEntities();


        // GET: BDonors

        public ActionResult Index()
        {
            // Get the donors for this user

            var username = User.Identity.Name;
            var userId = db.Userlogin.Where(u => u.Username == username).Select(u => u.Id).FirstOrDefault();

            ViewBag.UserId = userId;

            if (User.IsInRole("admin"))
            {

                return View(db.Bdonor.ToList());

            }
            else
            {
                // If the user is not an admin, show only their own details
                var donors = db.Bdonor.Where(b => b.UserId == userId).ToList();

                if (donors.Any())
                {
                    return View(donors);
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }

        }

        // GET: BDonors/Details/5
        //[Authorize(Roles = "user1,admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bdonor bDonors = db.Bdonor.Find(id);
            if (bDonors == null)
            {
                return HttpNotFound();
            }
            return View(bDonors);
        }

        // GET: BDonors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BDonors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BloodType,Phoneno,Address,Bloodneed")] Bdonor bDonors)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;

                // Fetch the user's ID from the Userlogin table
                var userId = db.Userlogin.Where(u => u.Username == username).Select(u => u.Id).FirstOrDefault();


                // Set UserId to the Id of the currently logged-in user
                bDonors.UserId = userId;
                db.Bdonor.Add(bDonors);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bDonors);
        }


        // GET: BDonors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bdonor bDonors = db.Bdonor.Find(id);
            if (bDonors == null)
            {
                return HttpNotFound();
            }
            return View(bDonors);
        }

        // POST: BDonors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BloodType,Phoneno,Address,Bloodneed")] Bdonor bDonors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bDonors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bDonors);
        }

        // GET: BDonors/Delete/5

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bdonor bDonors = db.Bdonor.Find(id);
            if (bDonors == null)
            {
                return HttpNotFound();
            }
            return View(bDonors);
        }

        // POST: BDonors/Delete/5

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bdonor bDonors = db.Bdonor.Find(id);
            db.Bdonor.Remove(bDonors);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
