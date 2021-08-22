DROP TABLE IF EXISTS `djangocms_snippet_snippet`;
CREATE TABLE `djangocms_snippet_snippet` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `html` longtext NOT NULL,
  `template` varchar(255) NOT NULL,
  `slug` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  UNIQUE KEY `djangocms_snippet_snippet_slug_bd43cd96_uniq` (`slug`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
