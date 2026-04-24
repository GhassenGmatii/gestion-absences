# 📊 Diagrammes UML - Système de Gestion des Absences

## Comment Visualiser les Diagrammes

Vous avez 2 options :

### Option 1️⃣ : PlantUML Online (Gratuit & Immédiat)

1. Allez sur : https://www.plantuml.com/plantuml/uml/
2. Copiez-collez le contenu d'un fichier `.puml`
3. Cliquez sur "Submit"
4. Téléchargez l'image (PNG, SVG, PDF)

### Option 2️⃣ : VS Code Extension

1. Installez l'extension : **PlantUML**
2. Ouvrez un fichier `.puml`
3. Appuyez sur `Alt + D` pour afficher l'aperçu
4. Clic droit → "Export Image"

### Option 3️⃣ : Ligne de Commande

```bash
# Installer PlantUML
choco install plantuml

# Convertir en PNG
plantuml -Tpng 01_UseCase_Diagram.puml
plantuml -Tpng 02_Class_Diagram.puml

# Convertir tous les fichiers
for %f in (*.puml) do plantuml -Tpng %f
```

---

## 📋 Fichiers UML Disponibles

| # | Fichier | Description | Type |
|---|---------|-------------|------|
| 1 | `01_UseCase_Diagram.puml` | Cas d'utilisation système | Use Case |
| 2 | `02_Class_Diagram.puml` | Structure des classes & DTOs | Class |
| 3 | `03_Sequence_Authentication.puml` | Flux d'authentification | Sequence |
| 4 | `04_Sequence_Absence.puml` | Flux de gestion des absences | Sequence |
| 5 | `05_Activity_Diagram.puml` | Workflow utilisateur | Activity |
| 6 | `06_State_Diagram.puml` | États d'une absence | State |
| 7 | `07_Component_Diagram.puml` | Architecture composants | Component |
| 8 | `08_Deployment_Diagram.puml` | Infrastructure déploiement | Deployment |

---

## 🎯 Diagrammes Inclus

### 1. Use Case (Diagramme de Cas d'Utilisation)
- ✅ 3 acteurs : Admin, Professeur, Étudiant
- ✅ 11 cas d'utilisation
- ✅ Relations (include, extend)

### 2. Class Diagram (Diagramme de Classes)
- ✅ 7 entités principales (User, Class, Student, Absence, Notification, RefreshToken)
- ✅ 8 services (interfaces + implémentations)
- ✅ 5 repositories
- ✅ 3 controllers
- ✅ DTOs complets
- ✅ Toutes les relations (1:1, 1:*, *:*)

### 3. Sequence Diagram - Authentication
- ✅ Flux Login complet
- ✅ JWT Token generation
- ✅ Token refresh
- ✅ Validation du token

### 4. Sequence Diagram - Absence Management
- ✅ Marquage des absences en masse
- ✅ Création des notifications
- ✅ Consultation des absences
- ✅ Interactions BD

### 5. Activity Diagram
- ✅ Workflow complet admin
- ✅ Workflow professeur
- ✅ Workflow étudiant
- ✅ Décisions et branches

### 6. State Diagram
- ✅ États d'une absence
- ✅ Transitions (Marquée → Justifiée → Archivée)
- ✅ Gestion des justifications

### 7. Component Diagram
- ✅ 8 couches architecturales
- ✅ Dépendances entre composants
- ✅ Cross-cutting concerns

### 8. Deployment Diagram
- ✅ Infrastructure cloud
- ✅ Kubernetes cluster
- ✅ Load balancing
- ✅ Réplication BD
- ✅ Cache & Message Queue
- ✅ Monitoring (Prometheus, Grafana, ELK)

---

## 🚀 Comment Utiliser

### Étape 1 : Télécharger les Diagrammes
```bash
git clone https://github.com/GhassenGmatii/gestion-absences.git
cd gestion-absences/UML_Diagrams
```

### Étape 2 : Convertir en Images
```bash
# Installer PlantUML CLI
pip install plantuml

# Convertir tous les fichiers
for file in *.puml; do
  plantuml -Tpng "$file"
  plantuml -Tsvg "$file"
  plantuml -Tpdf "$file"
done
```

### Étape 3 : Intégrer dans Documentation
```markdown
![Use Case Diagram](01_UseCase_Diagram.png)
![Class Diagram](02_Class_Diagram.png)
```

---

## 💡 Conseils

✅ **Pour les présentations** : Utilisez PNG (meilleure qualité)
✅ **Pour les rapports PDF** : Utilisez SVG (vectoriel, scalable)
✅ **Pour les wikis** : Utilisez PNG + lien vers fichier source
✅ **Pour les modifications** : Éditez les `.puml` directement

---

## 📧 Support

Pour des questions ou modifications des diagrammes :
- Consultez la documentation PlantUML : https://plantuml.com/
- Déposez une issue sur GitHub
- Consultez le fichier CONCEPTION.md pour les détails complets

**Tous les diagrammes sont prêts pour présentation professionnelle ! 🎓✅**
