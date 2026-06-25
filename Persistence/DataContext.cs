using Application.Common.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<KitUser, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{
    public DbSet<KitEvent> Events => Set<KitEvent>();
    public DbSet<KitSession> Sessions => Set<KitSession>();
    public DbSet<KitVideo> Videos => Set<KitVideo>();
    public DbSet<KitChat> Chats => Set<KitChat>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatEvent> ChatEvents => Set<ChatEvent>();
    public DbSet<ModerationEvent> ModerationEvents => Set<ModerationEvent>();
    public DbSet<ShowControl> ShowControls => Set<ShowControl>();
    public DbSet<UserAttending> UserAttendings => Set<UserAttending>();
    public DbSet<UserFollowing> UserFollowings => Set<UserFollowing>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureUsers(builder);
        ConfigureEvents(builder);
        ConfigureSessions(builder);
        ConfigureVideo(builder);
        ConfigureChat(builder);
        ConfigureAttendance(builder);
        ConfigureFollowing(builder);
    }

    private static void ConfigureUsers(ModelBuilder builder)
    {
        builder.Entity<KitUser>(entity =>
        {
            entity.Property(user => user.FirstName)
                .HasMaxLength(100);

            entity.Property(user => user.LastName)
                .HasMaxLength(100);

            entity.Property(user => user.Bio)
                .HasMaxLength(2_000);

            entity.Property(user => user.Industry)
                .HasMaxLength(100);

            entity.Property(user => user.Organization)
                .HasMaxLength(200);

            entity.Property(user => user.Title)
                .HasMaxLength(100);

            entity.Property(user => user.Country)
                .HasMaxLength(100);

            entity.Property(user => user.State)
                .HasMaxLength(100);

            entity.Property(user => user.City)
                .HasMaxLength(100);

            entity.Property(user => user.Interests)
                .HasMaxLength(1_000);

            entity.Property(user => user.Pronouns)
                .HasMaxLength(50);

            entity.Property(user => user.ProfileImage)
                .HasConversion(
                    uri => uri == null ? null : uri.ToString(),
                    value => value == null ? null : new Uri(value))
                .HasMaxLength(2_048);
        });
    }

    private static void ConfigureEvents(ModelBuilder builder)
    {
        builder.Entity<KitEvent>(entity =>
        {
            entity.Property(kitEvent => kitEvent.Title)
                .HasMaxLength(200);

            entity.Property(kitEvent => kitEvent.Description)
                .HasMaxLength(4_000);

            entity.Property(kitEvent => kitEvent.Image)
                .HasConversion(
                    uri => uri == null ? null : uri.ToString(),
                    value => value == null ? null : new Uri(value))
                .HasMaxLength(2_048);

            entity.HasMany(kitEvent => kitEvent.RunOfShow)
                .WithOne()
                .HasForeignKey(session => session.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(kitEvent => kitEvent.StartDate);
        });
    }

    private static void ConfigureSessions(ModelBuilder builder)
    {
        builder.Entity<KitSession>(entity =>
        {
            entity.Property(session => session.Title)
                .HasMaxLength(200);

            entity.Property(session => session.Description)
                .HasMaxLength(4_000);

            entity.Property(session => session.Category)
                .HasMaxLength(100);

            entity.Property(session => session.Sponsor)
                .HasMaxLength(200);

            entity.Property(session => session.AccessLevel)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(session => session.Image)
                .HasConversion(
                    uri => uri == null ? null : uri.ToString(),
                    value => value == null ? null : new Uri(value))
                .HasMaxLength(2_048);

            entity.HasOne(session => session.Speaker)
                .WithMany()
                .HasForeignKey(session => session.SpeakerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(session => session.Video)
                .WithOne(video => video.Session)
                .HasForeignKey<KitVideo>(video => video.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(session => session.Chat)
                .WithOne(chat => chat.Session)
                .HasForeignKey<KitChat>(chat => chat.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(session => session.EventId);
            entity.HasIndex(session => session.SpeakerId);
            entity.HasIndex(session => session.StartTime);
        });
    }

    private static void ConfigureVideo(ModelBuilder builder)
    {
        builder.Entity<KitVideo>(entity =>
        {
            entity.Property(video => video.PlaybackUrl)
                .HasConversion(
                    uri => uri.ToString(),
                    value => new Uri(value))
                .HasMaxLength(2_048);

            entity.Property(video => video.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasIndex(video => video.SessionId)
                .IsUnique();
        });
    }

    private static void ConfigureChat(ModelBuilder builder)
    {
        builder.Entity<KitChat>(entity =>
        {
            entity.HasIndex(chat => chat.SessionId)
                .IsUnique();

            entity.HasMany(chat => chat.Messages)
                .WithOne()
                .HasForeignKey(message => message.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(chat => chat.ChatEvents)
                .WithOne()
                .HasForeignKey(chatEvent => chatEvent.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(chat => chat.ModerationEvents)
                .WithOne()
                .HasForeignKey(moderationEvent => moderationEvent.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(chat => chat.ShowControls)
                .WithOne()
                .HasForeignKey(showControl => showControl.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ChatMessage>(entity =>
        {
            entity.Property(message => message.Body)
                .HasMaxLength(2_000);

            entity.HasOne(message => message.Author)
                .WithMany()
                .HasForeignKey(message => message.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(message => new { message.ChatId, message.TimeStamp });
        });

        builder.Entity<ChatEvent>(entity =>
        {
            entity.Property(chatEvent => chatEvent.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasOne(chatEvent => chatEvent.User)
                .WithMany()
                .HasForeignKey(chatEvent => chatEvent.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(chatEvent => new { chatEvent.ChatId, chatEvent.TimeStamp });
        });

        builder.Entity<ModerationEvent>(entity =>
        {
            entity.Property(moderationEvent => moderationEvent.Command)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasOne<KitUser>()
                .WithMany()
                .HasForeignKey(moderationEvent => moderationEvent.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(moderationEvent => new { moderationEvent.ChatId, moderationEvent.TimeStamp });
        });

        builder.Entity<ShowControl>(entity =>
        {
            entity.Property(showControl => showControl.Command)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasIndex(showControl => new { showControl.ChatId, showControl.TimeStamp });
        });
    }

    private static void ConfigureAttendance(ModelBuilder builder)
    {
        builder.Entity<UserAttending>(entity =>
        {
            entity.HasKey(attending => new { attending.UserId, attending.SessionId });

            entity.Property(attending => attending.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasOne(attending => attending.User)
                .WithMany(user => user.Attending)
                .HasForeignKey(attending => attending.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(attending => attending.Session)
                .WithMany(session => session.Attendees)
                .HasForeignKey(attending => attending.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(attending => attending.SessionId);
        });
    }

    private static void ConfigureFollowing(ModelBuilder builder)
    {
        builder.Entity<UserFollowing>(entity =>
        {
            entity.HasKey(following => new { following.ObserverId, following.TargetId });

            entity.HasOne(following => following.Observer)
                .WithMany(user => user.Following)
                .HasForeignKey(following => following.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(following => following.Target)
                .WithMany(user => user.Followers)
                .HasForeignKey(following => following.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(following => following.TargetId);
        });
    }
}
