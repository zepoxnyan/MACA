﻿using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class HomeController : Controller
    {
        /*private NewsDbContext dbNews = new NewsDbContext();
        private TeamMemberDbContext dbTeamMembers = new TeamMemberDbContext();
        private FunctionDbContext dbFunctions = new FunctionDbContext();*/
        PersonsDbContext dbPeople = new PersonsDbContext();
        MentorshipsDbContext dbMentorship = new MentorshipsDbContext();
        TeachingsDbContext dbTeaching = new TeachingsDbContext();
        PositionDbContext dbPositions = new PositionDbContext();
        PublicationsDbContext dbPublications = new PublicationsDbContext();

        public ActionResult Index()
        {
            //News lastNews = dbNews.News.OrderByDescending(x => x.DatePublished).First();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Press()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Papers()
        {
            return View();
        }

        public ActionResult People(string currentFilter, string searchString, int? page)
        {
            var persons = dbPeople.Persons.OrderBy(x => x.Surname).ThenBy(x => x.Name);

            /*
            var test = from person in dbPeople.Persons
                      where person.FullName.Contains("a")
                      select new {
                            Name = (person.Name.Length > person.Surname.Length ? person.Name : person.Surname)
                      };
            */

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                persons = persons.Where(m => m.Surname.Contains(searchString)
                                      || m.Name.Contains(searchString)
                                      || m.FullName.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(x => x.Surname).ThenBy(x => x.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["publicViewItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(persons.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult PersonDetails(Guid? personID = null)
        {
            if(personID == null)
                return RedirectToAction("People");

            Person person = dbPeople.Persons.Find(personID.Value);

            var positionList = dbPositions.Positions.Where(x => x.PersonID == personID).OrderByDescending(x => x.Year).ThenBy(x => x.Semester);
            if (positionList.Count() > 0)
                ViewBag.Position = positionList.First().PositionType.Name;
            else
                ViewBag.Position = "NAN";

            return View(person);
        }

        public ActionResult PersonMentorships(Guid personID)
        {
            List<Mentorship> lstMentorship = dbMentorship.Mentorships.Where(x => x.PersonID == personID).OrderBy(x => x.Year).ThenBy(x => x.Student).ToList();
            Person per = dbPeople.Persons.Find(personID);
            ViewBag.Title = per.Name + " " + per.Surname;
            ViewBag.PersonID = personID;
            return View(lstMentorship);
        }

        public ActionResult PersonTeaching(Guid personID)
        {
            List<Teaching> lstTeaching = dbTeaching.Teachings.Where(x => x.PersonID == personID).OrderByDescending(x => x.Subject.Year).ThenByDescending(x => x.Subject.Semester).ToList();
            Person per = dbPeople.Persons.Find(personID);
            ViewBag.Title = per.Name + " " + per.Surname;
            ViewBag.PersonID = personID;
            return View(lstTeaching);
        }

        public ActionResult PersonPublications(Guid personID)
        {
            List<Publication> lstPublications = new List<Publication>();
            Person per = dbPeople.Persons.Find(personID);
            ViewBag.Title = per.Name + " " + per.Surname;
            ViewBag.PersonID = personID;
            return View(lstPublications);
        }

        public ActionResult News()
        {
            //List<News> orderedNews = dbNews.News.OrderByDescending(x => x.DatePublished).ToList();
            return View();
        }

        public ActionResult Publications()
        {
            //List<TeamMember> teamMembers = dbTeamMembers.TeamMembers.OrderByDescending(x => x.PagePosition).ThenBy(x => x.LastName).ToList();
            return View();
        }

        public ActionResult FAQs()
        {
            FAQDbContext dbFAQs = new FAQDbContext();
            List<FAQ> sortedFAQs = dbFAQs.FAQs.OrderBy(x => x.Title).ToList();
            return View(sortedFAQs);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Administration()
        {
            ViewBag.Message = "Administration";

            return View();
        }

        /// <summary>
        /// Format:
        /// AISCode FirstName LastName FullName
        /// 5000020	Mirko	Horňák	prof. RNDr. Mirko Horňák, CSc.
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InsertPeople()
        {
            ViewBag.Message = "Insert People";

            //string path = "c:/MACA/persons-modified.txt";
            string path = ""; // To prevent bad actions!
            StreamReader srPeople = new StreamReader(path);
            List<Person> lstPeople = new List<Person>();

            while (!srPeople.EndOfStream)
            {
                string[] lineArr = srPeople.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Person person = new Person();
                person.PersonID = Guid.NewGuid();
                person.AISID = lineArr[0].Trim();
                person.Name = lineArr[1].Trim();
                person.Surname = lineArr[2].Trim();
                person.FullName = lineArr[3].Trim();
                lstPeople.Add(person);
            }
            srPeople.Close();

            // Insert Persons
            PersonsDbContext dbPersons = new PersonsDbContext();
            AuthorsDbContext dbAuthors = new AuthorsDbContext();

            foreach (Person person in lstPeople)
            {
                person.DateCreated = DateTime.Now;
                person.DateModified = DateTime.Now;
                person.UserCreatedID = User.Identity.GetUserId();
                person.UserModifiedID = person.UserCreatedID;

                dbPersons.Persons.Add(person);
                dbPersons.SaveChanges();

                // Automatically add a person to authors
                Author author = new Author();
                author.AuthorID = Guid.NewGuid();
                author.Surname = person.Surname;
                author.FirstName = person.Name;

                author.DateCreated = DateTime.Now;
                author.DateModified = DateTime.Now;
                author.UserCreatedID = new Guid(User.Identity.GetUserId());
                author.UserModifiedID = author.UserCreatedID;

                dbAuthors.Authors.Add(author);
                dbAuthors.SaveChanges();

                Person personTmp = dbPersons.Persons.Find(person.PersonID);
                personTmp.AuthorID = author.AuthorID;
                dbPersons.Entry(personTmp).State = EntityState.Modified;
                dbPersons.SaveChanges();
            }

            return RedirectToAction("Administration");
        }

        /// <summary>
        /// Format:
        /// AISCode Typ RokStart TypPrace Meno Prezvisko Nazov
        /// 5000108	1	2010/2011	1	Hucovičová	Gabriela	História matematiky v kontexte výučby matematiky na základnej škole
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InsertMentors()
        {
            ViewBag.Message = "Insert Mentors";

            //string path = "c:/MACA/mentors.txt";
            string path = "";
            //string path = ""; // To prevent bad actions!
            StreamReader srMentors = new StreamReader(path);
            List<Mentorship> lstMentors = new List<Mentorship>();
            MentorshipsDbContext dbMentors = new MentorshipsDbContext();
            PersonsDbContext dbPersons = new PersonsDbContext();
            MentorshipTypeDbContext dbMentorshipType = new MentorshipTypeDbContext();
            ThesisTypeDbContext dbThesisType = new ThesisTypeDbContext();

            while (!srMentors.EndOfStream)
            {
                string[] lineArr = srMentors.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Mentorship mentor = new Mentorship();
                mentor.MentorshipID = Guid.NewGuid();
                string personCode = lineArr[0].Trim();
                mentor.PersonID = dbPersons.Persons.Where(x => x.AISID == personCode).First().PersonID;
                int mentortype = int.Parse(lineArr[1].Trim());
                mentor.MentorshipTypeID = dbMentorshipType.MentorshipTypes.Where(x => x.AISCode == mentortype).First().MentorshipTypeID;
                mentor.Year = lineArr[2].Trim();
                int thesistype = int.Parse(lineArr[3].Trim());
                mentor.ThesisTypeID = dbThesisType.ThesisTypes.Where(x => x.AISCode == thesistype).First().ThesisTypeID;
                mentor.Student = lineArr[4].Trim() + " " + lineArr[5].Trim();
                mentor.ThesisTitle = lineArr[6].Trim();
                lstMentors.Add(mentor);
            }
            srMentors.Close();

            // Insert Persons
            

            foreach (Mentorship mentor in lstMentors)
            {
                mentor.DateCreated = DateTime.Now;
                mentor.DateModified = DateTime.Now;
                mentor.UserCreatedID = new Guid(User.Identity.GetUserId());
                mentor.UserModifiedID = mentor.UserCreatedID;

                dbMentors.Mentorships.Add(mentor);
                dbMentors.SaveChanges();
            }

            return RedirectToAction("Administration");
        }

        /// <summary>
        /// Format:
        /// AISCode Semester Rok Typ PočetHodin AISCode Dep/Skratka Nazov
        /// 3326	L	2013/2014	C	3	14676283	ÚMV/MANb	Matematická analýza II
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InsertSubjects()
        {
            ViewBag.Message = "Insert Subjects";

            string path = "c:/MACA/teachings.txt";
            //string path = ""; // To prevent bad actions!
            StreamReader srTeachings = new StreamReader(path);
            List<TMPSubject> lstTMPSubjects = new List<TMPSubject>();

            while (!srTeachings.EndOfStream)
            {
                string[] lineArr = srTeachings.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                TMPSubject subject = new TMPSubject();
                subject.TeacherCode = lineArr[0].Trim();
                int sem = lineArr[1].Trim().ToLower() == "l" ? 2 : 1;
                subject.Semester = sem;
                subject.Year = lineArr[2].Trim();
                subject.TeachingType = lineArr[3].Trim();
                subject.HoursPerWeek = double.Parse(lineArr[4].Trim());
                subject.SubjectCode = lineArr[5].Trim();
                string depShort = lineArr[6].Trim();
                string[] depShortSplit = depShort.Split('/');
                subject.Department = depShortSplit[0].Trim();
                subject.ShortName = depShortSplit[1].Trim();
                subject.Name = lineArr[7].Trim();

                if(lstTMPSubjects.Where(x => x.ShortName == subject.ShortName && x.Department == subject.Department && x.TeacherCode == subject.TeacherCode
                            && x.TeachingType == subject.TeachingType && x.Year == subject.Year && x.Semester == subject.Semester && x.SubjectCode == subject.SubjectCode).Count() > 0)
                {
                    lstTMPSubjects.Where(x => x.ShortName == subject.ShortName && x.Department == subject.Department && x.TeacherCode == subject.TeacherCode
                            && x.TeachingType == subject.TeachingType && x.Year == subject.Year && x.Semester == subject.Semester && x.SubjectCode == subject.SubjectCode)
                            .First().HoursPerWeek += subject.HoursPerWeek;
                }
                else
                    lstTMPSubjects.Add(subject);
            }
            srTeachings.Close();

            // Insert Persons
            PersonsDbContext dbPersons = new PersonsDbContext();
            SubjectsDbContext dbSubjects = new SubjectsDbContext();
            TeachingsDbContext dbTeachings = new TeachingsDbContext();

            List<Subject> lstSubjects = new List<Subject>();
            Dictionary<Guid, List<Teaching>> dicTeachings = new Dictionary<Guid, List<Teaching>>();

            foreach(TMPSubject tmp in lstTMPSubjects)
            {                
                if (lstSubjects.Where(x => x.ShortName == tmp.ShortName && x.Department == tmp.Department && x.Year == tmp.Year 
                    && x.Semester == tmp.Semester && x.AISCode == tmp.SubjectCode).Count() == 0)
                {
                    Subject sub = new Subject();
                    sub.SubjectID = Guid.NewGuid();
                    sub.AISCode = tmp.SubjectCode;
                    sub.Department = tmp.Department;
                    sub.Name = tmp.Name;
                    sub.Semester = tmp.Semester;
                    sub.ShortName = tmp.ShortName;
                    sub.AISCode = tmp.SubjectCode;
                    sub.Year = tmp.Year;
                    lstSubjects.Add(sub);

                    Teaching t = new Teaching();
                    t.TeachingID = Guid.NewGuid();
                    t.Hours = tmp.HoursPerWeek;
                    try
                    {
                        t.PersonID = dbPersons.Persons.Where(x => x.AISID == tmp.TeacherCode).First().PersonID;
                    }
                    catch
                    {
                        continue;
                    }
                    t.SubjectID = sub.SubjectID;
                    t.TeachingTypeID = dbTeachings.TeachingTypes.Where(x => x.AISCode == tmp.TeachingType).First().TeachingTypeID;

                    dicTeachings.Add(sub.SubjectID, new List<Teaching>());
                    dicTeachings[sub.SubjectID].Add(t);
                }
                else
                {
                    Subject sub = lstSubjects.Where(x => x.ShortName == tmp.ShortName && x.Department == tmp.Department && x.Year == tmp.Year
                    && x.Semester == tmp.Semester && x.AISCode == tmp.SubjectCode).First();

                    Teaching t = new Teaching();
                    t.TeachingID = Guid.NewGuid();
                    t.Hours = tmp.HoursPerWeek;
                    try
                    {
                        t.PersonID = dbPersons.Persons.Where(x => x.AISID == tmp.TeacherCode).First().PersonID;
                    }
                    catch
                    {
                        continue;
                    }
                    t.SubjectID = sub.SubjectID;
                    t.TeachingTypeID = dbTeachings.TeachingTypes.Where(x => x.AISCode == tmp.TeachingType).First().TeachingTypeID;

                    dicTeachings[sub.SubjectID].Add(t);
                }
            }

            foreach (Subject sub in lstSubjects)
            {
                sub.DateCreated = DateTime.Now;
                sub.DateModified = DateTime.Now;
                sub.UserCreatedID = new Guid(User.Identity.GetUserId());
                sub.UserModifiedID = sub.UserCreatedID;

                dbSubjects.Subjects.Add(sub);
                dbSubjects.SaveChanges();

                if (dicTeachings.ContainsKey(sub.SubjectID))
                {
                    foreach (Teaching t in dicTeachings[sub.SubjectID])
                    {
                        t.DateCreated = DateTime.Now;
                        t.DateModified = DateTime.Now;
                        t.UserCreatedID = new Guid(User.Identity.GetUserId());
                        t.UserModifiedID = t.UserCreatedID;

                        dbTeachings.Teachings.Add(t);
                        dbTeachings.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Administration");
        }

        public class TMPSubject
        {
            public string TeacherCode { get; set; }
            public int Semester { get; set; }
            public string Year { get; set; }
            public string Department { get; set; }
            public string ShortName { get; set; }
            public string SubjectCode { get; set; }
            public string Name { get; set; }
            public string TeachingType { get; set; }
            public double HoursPerWeek { get; set; }
        }
    }
}