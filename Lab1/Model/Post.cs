using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    /// <summary>
    /// Klass för att representera t.ex. en bloggpost eller en forumpost.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Tom konstruktor - alla variabler/properties får default-värden om denna används
        /// </summary>
        public Post() { }

        /// <summary>
        /// Konstruktor för ett giltigt Post-objekt
        /// </summary>
        /// <param name="createdByID">UserID för den User som skapat posten</param>
        /// <param name="title">Titel för posten</param>
        /// <param name="body">Body/Meddelande för posten</param>
        /// <param name="parent">[Optional, Default: null] Den överliggande posten</param>
        public Post(Guid createdByID, string title, string body, Post parent = null)
        {
            PostID = Guid.NewGuid();
            CreatedByID = createdByID;
            Title = title;
            Body = body;
            ParentPost = parent;
            CreateDate = DateTime.UtcNow;
        }
        // Unikt ID för en Post
        public Guid PostID { get; set; }

        // Privat referens till det User-objekt som skapade posten
        private User _CreatedBy { get; set; }
        // UserID för det User-objekt som skapade posten
        public Guid CreatedByID { get; set; }
        // Publikt åtkomlig referens till det User-objekt som skapade posten
        public User CreatedBy { get { return _CreatedBy; } }

        // Datumet då posten skapades
        public DateTime CreateDate { get; set; }
        // Titel för posten
        public string Title { get; set; }
        // Body/Innehåll för posten
        public string Body { get; set; }
        // Referens till överliggande post (tänk forum)
        public Post ParentPost { get; set; }
        // En lista över Tags på en post (PostTags är en enum som definieras längre ned)
        public List<PostTags> Tags { get; set; }

        // Returnerar en strängrepresentation av de PostTags som ett Post-objekt är taggat med
        public string FormattedTagList
        {
            get
            {
                if (Tags.Count == 0)
                    return "";

                string tagListString = "";
                foreach (var tag in Tags)
                {
                    if (!string.IsNullOrEmpty(tagListString))
                        tagListString += ", ";
                    tagListString += tag;
                }

                return tagListString;
            }
        }

        /// <summary>
        /// Returnerar en strängrepresentation av ett Post-objekt
        /// </summary>
        /// <param name="ShortFormat">Om denna sätts till false kommer mer detaljerad information tas med.</param>
        /// <returns></returns>
        public string ToString(bool ShortFormat = true)
        {
            string postString = string.Format("\tTitle: '{0}' - By: '{1}' - Posted: '{2:d}'", Title, CreatedBy != null ? CreatedBy.UserName : "?", CreateDate);
            if (!ShortFormat)
                postString += string.Format("\n\t\t Message: {0}\n\t\t Tags: {1}", Body, FormattedTagList);

            return postString;
        }

        /*
         * OBS! Denna konstruktion för att ladda relaterad data är varken en bra eller vanlig konstruktion.
         * 
         * Vi kommer se hur detta kan skötas i verkligheten senare i kursen
         * 
         */
        /// <summary>
        /// Ladda in en User-referens till ett Post-objekt
        /// </summary>
        /// <param name="users">En lista över alla giltiga User-objekt i applikationen</param>
        public void LoadUser(List<User> users)
        {
            _CreatedBy = users.Where(u => u.UserID == CreatedByID).FirstOrDefault();
        }

        /// <summary>
        /// Tags för att beskriva en Post
        /// </summary>
        public enum PostTags
        {
            Interesting,
            Funny,
            Troll,
            JawDropping,
            Useful
        }
    }
}
